using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchingProduct
{
	public enum WhatProduct
	{
		Cash500,
		Cash1000,
		Cash5000,
		NoAds,
        unlockallgame,
		Levels,
		characters
	}

	public WhatProduct product;
}
