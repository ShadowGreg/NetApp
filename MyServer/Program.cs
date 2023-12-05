namespace MyServer {
    public static class Program {
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        static CancellationToken cancellationToken = cancellationTokenSource.Token;
        public static async Task Main() {
            var server = new MyServer(3130);
            await server.Start(cancellationToken);
            
            // To stop the server, call:
            await cancellationTokenSource.CancelAsync();
        }
    }
}