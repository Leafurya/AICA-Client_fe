using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pronunciation
{
    public sealed class WsClient : IAsyncDisposable
    {
        private ClientWebSocket? _ws;
        private CancellationTokenSource? _recvCts;


        public event Action<string>? OnText;
        public event Action<WebSocketCloseStatus?, string?>? OnClosed;


        public async Task ConnectAsync(Uri uri, string? bearerToken = null, CancellationToken ct = default)
        {
            _ws = new ClientWebSocket();
            if (!string.IsNullOrEmpty(bearerToken))
                _ws.Options.SetRequestHeader("Authorization", $"Bearer {bearerToken}");


            await _ws.ConnectAsync(uri, ct);
            _recvCts = new CancellationTokenSource();
            _ = Task.Run(() => ReceiveLoop(_recvCts.Token));
        }


        public async Task SendTextAsync(string text, CancellationToken ct = default)
        {
            if (_ws is not { State: WebSocketState.Open }) return;
            var seg = new ArraySegment<byte>(Encoding.UTF8.GetBytes(text));
            await _ws.SendAsync(seg, WebSocketMessageType.Text, true, ct);
        }


        public async Task SendBinaryAsync(byte[] data, CancellationToken ct = default)
        {
            if (_ws is not { State: WebSocketState.Open }) return;
            var seg = new ArraySegment<byte>(data);
            await _ws.SendAsync(seg, WebSocketMessageType.Binary, true, ct);
        }


        private async Task ReceiveLoop(CancellationToken ct)
        {
            var buffer = new byte[8192];
            try
            {
                while (!ct.IsCancellationRequested && _ws is { State: WebSocketState.Open })
                {
                    var result = await _ws!.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "client ack", CancellationToken.None);
                        OnClosed?.Invoke(_ws.CloseStatus, _ws.CloseStatusDescription);
                        break;
                    }


                    int count = result.Count;
                    while (!result.EndOfMessage)
                    {
                        if (count >= buffer.Length) Array.Resize(ref buffer, buffer.Length * 2);
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer, count, buffer.Length - count), ct);
                        count += result.Count;
                    }


                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var text = Encoding.UTF8.GetString(buffer, 0, count);
                        OnText?.Invoke(text);
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                OnText?.Invoke(JsonSerializer.Serialize(new
                {
                    type = "error",
                    message = ex.Message
                }));
                OnClosed?.Invoke(_ws?.CloseStatus, ex.Message);
            }
        }


        public async ValueTask DisposeAsync()
        {
            try
            {
                _recvCts?.Cancel();
                if (_ws is { State: WebSocketState.Open })
                    await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", CancellationToken.None);
                _ws?.Dispose();
            }
            catch { }
        }
        public async Task CloseAsync(string reason = "client request", CancellationToken ct = default)
        {
            try
            {
                if (_ws is { State: WebSocketState.Open })
                {
                    await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, reason, ct);
                    OnClosed?.Invoke(WebSocketCloseStatus.NormalClosure, reason);
                }
            }
            catch (Exception ex)
            {
                OnClosed?.Invoke(WebSocketCloseStatus.InternalServerError, ex.Message);
            }
            finally
            {
                _recvCts?.Cancel();
                _ws?.Dispose();
            }
        }

    }
}
