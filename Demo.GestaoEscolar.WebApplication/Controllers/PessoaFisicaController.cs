﻿using CrossCutting;
using Demo.GestaoEscolar.Domain.Finders;
using Demo.GestaoEscolar.Domain.Finders.Dtos;
using Demo.GestaoEscolar.Domain.Services.PessoasFisicas;
using Demo.GestaoEscolar.WebApplication.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.GestaoEscolar.WebApplication.Controllers
{
	[Produces("application/json")]
	[Route("api/pessoas-fisicas")]
	public class PessoaFisicaController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPessoaFisicaService _pessoaFisicaService;
		private readonly IPessoaFisicaFinder _pessoaFisicaFinder;

		public PessoaFisicaController(IUnitOfWork unitOfWork,
									 IPessoaFisicaService pessoaFisicaService,
									 IPessoaFisicaFinder pessoaFisicaFinder)
		{
			_unitOfWork = unitOfWork;
			_pessoaFisicaService = pessoaFisicaService;
			_pessoaFisicaFinder = pessoaFisicaFinder;
		}

		[HttpPost]
		[Route("")]
		public async Task<Guid> CriarAsync([FromBody]CriarPessoaFisicaDto dto)
		{
			var id = Guid.NewGuid();

			await _pessoaFisicaService.CriarAsync(id, dto.Nome, dto.Cpf, dto.NomeSocial, dto.Sexo, dto.DataNascimento);
			await _unitOfWork.CommitAsync();

			return id;

		}

		[HttpPut]
		[Route("{id:guid}/alterar-cpf")]
		public async Task<Guid> AlterarCpfAsync(Guid id, string cpf)
		{
			await _pessoaFisicaService.AlterarCpfAsync(id, cpf);
			await _unitOfWork.CommitAsync();

			return id;

		}

		[HttpGet]
		[Route("")]
		public async Task<IEnumerable<PessoaFisicaDto>> ObterAsync()
		{
			var result = await _pessoaFisicaFinder.ObterAsync();
			if (result == null || !result.Any()) NotFound();

			return result;

		}
	}
}
