//using OrionBank.Client.Extensions;

//namespace OrionBank.Client.Services
//{
//    public class BaseClusterService
//    {
//        private readonly IClusterClient _clusterClient;
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public BaseClusterService(IClusterClient clusterClient, IHttpContextAccessor httpContextAccessor)
//        {
//            _clusterClient = clusterClient;
//            _httpContextAccessor = httpContextAccessor;
//        }


//        protected T TryUseGrain<TGrainInterface, T>(
//            Func<TGrainInterface, T> useGrain, Func<T> defaultValue)
//            where TGrainInterface : IGrainWithStringKey =>
//             TryUseGrain(
//                 useGrain,
//                 _httpContextAccessor.TryGetUserId(),
//                 defaultValue);

//        protected T TryUseGrain<TGrainInterface, T>(
//            Func<TGrainInterface, T> useGrain,
//            string? key,
//            Func<T> defaultValue)
//            where TGrainInterface : IGrainWithStringKey =>
//            key is { Length: > 0 } primaryKey
//                ? useGrain.Invoke(_clusterClient.GetGrain<TGrainInterface>(primaryKey))
//                : defaultValue.Invoke();
//    }
//}
