namespace Shared.Models
{
    public struct WordCount
    {
        public int Count { get; }
        public string TextId { get; }

        public WordCount(int count, string textId)
        {
            Count = count;
            TextId = textId;
        }

        public override string ToString()
        {
            return $"{Count} words in {TextId}";
        }
    }
}
