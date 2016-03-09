using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Infra.Servicos;
using Infra.Interfaces;

namespace appRike.IoC
{
    public class ModuloNinject
    {
        private IKernel ninjectKernel = new StandardKernel();

        public ModuloNinject()
        {
            Configuracao();
        }

        public void Configuracao()
        {
            ninjectKernel.Bind<IServicoDeAgenda>().To<ServicoDeAgenda>();
        }

        public IServicoDeAgenda InstanciaServicoAgenda()
        {
            return ninjectKernel.Get<IServicoDeAgenda>();
        }
    }
}
