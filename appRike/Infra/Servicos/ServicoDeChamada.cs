﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dominio.Entidades;
using Infra.Contexto;

namespace Infra.Servicos
{
    public class ServicoDeChamada
    {
        private readonly DbContexto _db;

        public ServicoDeChamada()
        {
            _db = new DbContexto();
        }

        public bool Adicionar(Chamada chamada)
        {
            _db.Chamadas.Add(chamada);
            return _db.SaveChanges() > 0;
        }

        public bool Alterar(Chamada chamada)
        {
            _db.Entry(chamada).State = EntityState.Modified;
            return _db.SaveChanges() > 0;
        }

        public bool Excluir(int chamadaId)
        {
            _db.Chamadas.Remove(_db.Chamadas.Find(chamadaId));
            return _db.SaveChanges() > 0;
        }

        public Chamada GetById(int id)
        {
            return _db.Set<Chamada>().Find(id);
        }

        public List<Chamada> Get(Func<Chamada, bool> filtro, params string[] predicate)
        {
            return GetAll(predicate).Where(filtro).ToList();
        }

        public List<Chamada> GetAll(params string[] predicate)
        {
            IQueryable<Chamada> query = _db.Set<Chamada>();
            query = predicate.Aggregate(query, (current, item) => current.Include(item));
            return query.OrderBy(x => x.Id).ToList();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
