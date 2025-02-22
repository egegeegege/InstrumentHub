﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstrumentHub.Entites;

namespace Instrument.Business.Abstract
{
	public interface ICommentServices
	{
		Comment GetById(int id);
		void Create(Comment entity);
		void Update(Comment entity);
		void Delete(Comment entity);
		double GetAverageRating(int eproductId);
		List<Comment> GetCommentsByProductId(int eproductId);
	}
}
