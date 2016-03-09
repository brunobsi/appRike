using Ninject;
using Infra.Servicos;
using Infra.Interfaces;

namespace BNR_ComputerClass.IoC
{
    public class ModuloNinject
    {
        private readonly IKernel _ninjectKernel = new StandardKernel();

        public ModuloNinject()
        {
            Configuracao();
        }

        public void Configuracao()
        {
            _ninjectKernel.Bind<IServicoDeAgenda>().To<ServicoDeAgenda>();
        }

        public IServicoDeAgenda InstanciaServicoAgenda()
        {
            return _ninjectKernel.Get<IServicoDeAgenda>();
        }
    }
}
