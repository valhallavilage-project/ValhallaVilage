using CrossProject.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm.Features.QuestIndication
{
    public class QuestIndicationService : IInitializable
    {
        private readonly AddressablesManager _addressablesManager;

        

        public bool IsInitialized { get; private set; }

        public QuestIndicationService(AddressablesManager addressablesManager)
        {
            _addressablesManager = addressablesManager;
        }

        public async UniTask Initialize()
        {
            
            IsInitialized = true;
        }
    }
}
