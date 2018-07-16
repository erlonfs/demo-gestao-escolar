﻿using Demo.GestaoEscolar.Domain.Aggregates.Alunos;
using SharedKernel.Common;
using System;
using System.Collections.Generic;

namespace Demo.GestaoEscolar.Domain.Aggregates.Escolas
{
	public class Sala : Entity<Guid>
	{
		public int Id { get; private set; }

		public int EscolaId { get; private set; }
		public virtual Escola Escola { get; private set; }

		public string FaseAno { get; private set; }
		public int TurnoId { get; private set; }

		public virtual HashSet<SalaAluno> Alunos { get; private set; } = new HashSet<SalaAluno>();

		protected Sala()
		{

		}

		internal Sala(Guid id, string faseAno, Turno turno)
		{
			EntityId = id;
			FaseAno = faseAno;
			TurnoId = (int)turno;
		}

		internal void AdicionarAluno(Aluno aluno)
		{
			Alunos.Add(new SalaAluno(this, aluno));
		}
	}
}
