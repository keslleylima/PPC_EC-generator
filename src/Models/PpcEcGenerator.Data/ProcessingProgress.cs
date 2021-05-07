namespace PpcEcGenerator.Data
{
    public class ProcessingProgress
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        public ProcessingProgress(int start, int end, int current)
        {
            Start = start;
            End = end;
            Current = current;
        }


        //---------------------------------------------------------------------
        //		Properties
        //---------------------------------------------------------------------
        public int Start { get; private set; }
        public int End { get; private set; }
        public int Current { get; set; }


        //---------------------------------------------------------------------
        //		Methods
        //---------------------------------------------------------------------
        public void Forward()
        {
            if (Current + 1 > End)
                return;

            Current++;
        }
    }
}
