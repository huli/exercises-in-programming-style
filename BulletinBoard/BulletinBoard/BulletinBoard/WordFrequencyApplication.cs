namespace BulletinBoardStyle
{
    internal class WordFrequencyApplication : BoardMember
    {
        public WordFrequencyApplication(BulletinBoard board)
            :base(board)
        {
            Board.Subscribe("run", OnRun);
            Board.Subscribe("eof", OnStop);
        }

        private void OnStop(object sender, DynamicEventArgs e)
        {
            Board.Publish("print", DynamicEventArgs.Empty);
        }

        public void OnRun(object sender, DynamicEventArgs args)
        {
            Board.Publish("load", DynamicEventArgs.Empty);
            Board.Publish("start", DynamicEventArgs.Empty);
        }
    }
}