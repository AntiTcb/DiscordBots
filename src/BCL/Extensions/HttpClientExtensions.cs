#region Header

// Description:
// 
// Solution: DiscordBots
// Project: BCL
// 
// Created: 10/19/2016 9:54 PM
// Last Revised: 10/19/2016 9:55 PM
// Last Revised by: Alex Gravely

#endregion

namespace BCL.Extensions {
    #region Using

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    #endregion

    public static class HttpClientExtensions {
        #region Public Methods

        public static async Task DownloadAsync(this HttpClient client, Uri requestUri, string filename) {
            using (client = new HttpClient()) {
                using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri)) {
                    using (
                        Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(),
                               stream = new FileStream
                                   (filename, FileMode.Create, FileAccess.Write, FileShare.None, 3145728, true)) {
                        await contentStream.CopyToAsync(stream).ConfigureAwait(false);
                    }
                }
            }
        }

        #endregion Public Methods
    }
}