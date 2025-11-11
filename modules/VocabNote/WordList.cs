using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using Utility.Data.Json;
using Utility.Data.Word;

namespace VocabNote
{
    class WordList
    {
        private GetWordBody body;
        private List<VocabItem> data=new List<VocabItem>();
        private List<int> wordIds = new List<int>();
        private Dictionary<int, List<VocabItem>> aicaList = new Dictionary<int, List<VocabItem>>();
        public WordList(string stringifiedBody)
        {
            Debug.WriteLine("stringifiedBody " + stringifiedBody);
            body = JsonSerializer.Deserialize<GetWordBody>(stringifiedBody);
            if (body.code != 200)
            {
                Debug.WriteLine(body.message);
                return;
            }
            if (body != null)
            {
                List<VocabItem>  tempData = body.data;
                tempData.ForEach(item => {
                    AddToVocabList(item);
                    AddToAicaList(item);
                });
            }
        }
        public WordList()
        {
            data = new List<VocabItem>();
        }
        public void Debug_ShowList()
        {
            if (data == null)
            {
                Debug.WriteLine("word list is null");
                return;
            }
            data.ForEach(item =>
            {
                Debug.WriteLine(item.word + " " + item.wordId);
            });

        }
        public List<VocabItem> GetAicaList(int sentenceId)
        {
            try
            {
                return aicaList[sentenceId];
            }
            catch (Exception ex)
            {
                return new List<VocabItem>();
            }
        }
        public List<VocabItem> GetWordList()
        {
            return data;
        }
        public (bool,bool) Add(VocabItem vocabItem)
        {
            bool vocabAddResult=AddToVocabList(vocabItem);
            bool aicaAddResult = AddToAicaList(vocabItem);
            return (vocabAddResult, aicaAddResult);
        }
        private bool AddToAicaList(VocabItem vocabItem)
        {

            int sentenceId = vocabItem.sentenceId??-1;
            if (sentenceId == -1)
            {
                return false;
            }

            if (!aicaList.ContainsKey(sentenceId))
            {
                aicaList[sentenceId] = new List<VocabItem>();
            }
            
            if (!IsExistAtAicaList(sentenceId, vocabItem.wordId))
            {
                aicaList[sentenceId].Add(vocabItem);
                return true;
            }
            return false;
        }
        private bool AddToVocabList(VocabItem vocabItem)
        {
            Debug.WriteLine($"id: {vocabItem.wordId} {vocabItem.sentenceId} {wordIds.Contains(vocabItem.wordId)}");
            if (!wordIds.Contains(vocabItem.wordId))
            {
                data.Add((VocabItem)vocabItem);
                wordIds.Add(vocabItem.wordId);
                return true;
            }
            return false;
        }
        public bool IsExistAtAicaList(int textId, int wordId)
        {
            try
            {
                for (int i = 0; i < aicaList[textId].Count; i++)
                {
                    if (aicaList[textId][i].wordId == wordId)
                    {
                        return true;
                    }
                }
                return false;
            }catch { return false; }
        }
        public void DeleteWord(int wordId)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].wordId == wordId)
                {
                    Debug.WriteLine($"idx: {i}");
                    data.RemoveAt(i);
                    break;
                }
            }

            data.ForEach((VocabItem vocabItem) =>
            {
                Debug.WriteLine($"있는 단어 {vocabItem.word}");
            });
            try
            {
                foreach (var list in aicaList)
                {
                    for (int i = 0; i < list.Value.Count; i++)
                    {
                        if (list.Value[i].wordId == wordId)
                        {
                            Debug.WriteLine($"aica idx: {i}");
                            aicaList[list.Key].RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            wordIds.Remove(wordId);
        }
        public void ClearAicaList(int textId)
        {
            aicaList[textId]=new List<VocabItem>();
        }
    }
}
