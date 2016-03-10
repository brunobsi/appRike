using Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace Infra.Interfaces
{
    public interface IServicoDeAgenda
    {
        bool Adicionar(Agenda agenda);

        bool Excluir(Agenda agenda);

        Agenda GetById(int id);

        List<Agenda> Get(Func<Agenda, bool> filtro, params string[] predicate);

        List<Agenda> GetAll(params string[] predicate);

        bool VerificarSePodeAgendar(Agenda agenda);

        void Dispose();
    }
}
