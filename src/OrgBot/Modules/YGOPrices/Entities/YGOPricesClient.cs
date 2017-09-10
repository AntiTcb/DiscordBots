namespace OrgBot.Modules.YGOPrices.Entities
{
    using RestEase;
    using System.Net;
    using System.Threading.Tasks;

    public static class YGOPricesClient
    {
        internal static readonly IYGOPricesAPI API = RestClient.For<IYGOPricesAPI>(BASE_URI);

        const string BASE_URI = "http://yugiohprices.com/api/";

        public static async Task<YGOPricesCard?> GetCardAndPriceResponseAsync(string cardName)
        {
            try
            {
                var dataResponse = await API.GetCardDataAsync(cardName);
                var priceResponse = await API.GetCardPriceAsync(cardName);
                return new YGOPricesCard(dataResponse, priceResponse);
            }
            catch (ApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

    }
}