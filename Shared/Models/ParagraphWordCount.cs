namespace Shared.Models
{
    public struct ParagraphWordCount
    {
        public int Count { get; }
        public string TextId { get; }

        public ParagraphWordCount(int count, string textId)
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
