namespace TM.Digital.Model.Prerequisite
{
    public class Prerequisite
    {
        public int Value { get; set; }

        public PrerequisiteKind PrerequisiteKind { get; set; }
        public bool IsMax { get; set; }
    }
}
