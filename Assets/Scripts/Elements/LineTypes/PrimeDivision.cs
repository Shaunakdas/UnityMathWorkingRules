using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimeDivision  {
	public int Target{ get; set; }
	public List<int> PrimeFactorList{ get; set; }
	public List<int> PrimeDividendList{ get; set; }
	public PrimeDivision(int target){
		PrimeFactorList = new List<int> (); PrimeDividendList = new List<int> ();
		Target = target;
		PrimeFactorList = GeneratePrimeList (target);
		PrimeDividendList = GenerateDividendList (target, PrimeFactorList);
	}
	public List<int> GeneratePrimeList(int number){
		var primes = new List<int>();

		for(int div = 2; div<=number; div++){
			while(number%div==0){
				primes.Add(div);
				number = number / div;
			}
		}
		return primes;
	}
	public List<int> GenerateDividendList(int number, List<int> primeFactorList){
		var dividends = new List<int>();
		dividends.Add (number);
		foreach (int factor in primeFactorList) {
			Debug.Log ("PRIME_DIVIDEND" + (number / factor));
			dividends.Add (number/factor);
			number = number / factor;
		}
		return dividends;
	}
}
