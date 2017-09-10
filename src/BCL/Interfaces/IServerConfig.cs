namespace BCL.Interfaces {

    using System.Collections.Generic;

    public interface IServerConfig {

        string CommandPrefix { get; set; }
        Dictionary<string, string> Tags { get; set; }

    }
}