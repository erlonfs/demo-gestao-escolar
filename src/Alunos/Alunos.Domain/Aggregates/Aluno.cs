﻿using SharedKernel.Common;
using System;

namespace Alunos.Domain.Aggregates
{
	public class Aluno : Entity<Guid>
	{
		public int Id { get; private set; }
		public DateTime DataCriacao { get; private set; }

		public int PessoaFisicaId { get; private set; }
		public virtual PessoaFisica PessoaFisica { get; private set; }

		public int Matricula { get; private set; }

		protected Aluno()
		{

		}

		public Aluno(Guid id, DateTime dataCriacao, int pessoaFisicaId, int matricula)
		{
			EntityId = id;
			DataCriacao = dataCriacao;
			PessoaFisicaId = pessoaFisicaId;
			Matricula = matricula;

			DomainEvents.Raise(new AlunoCriado(EntityId, this));

		}
	}
}
