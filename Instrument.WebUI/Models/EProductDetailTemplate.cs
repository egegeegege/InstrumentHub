﻿using InstrumentHub.Entites;

namespace Instrument.WebUI.Models
{
	public class EProductDetailTemplate
	{
		public EProduct EProduct { get; set; }
		public List<Division> Divisions { get; set; }
		public List<Comment> Comments { get; set; }
	}
}
