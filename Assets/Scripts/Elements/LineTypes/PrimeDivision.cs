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
	public static bool isPrime(int number)
	{

		if (number == 1) return false;
		if (number == 2) return true;

		for (int i = 2; i <= Mathf.Ceil(Mathf.Sqrt(number)); ++i)  {
			if (number % i == 0)  return false;
		}

		return true;

	}
	public static int gcd(int first , int second){
		int a = first;
		int b = second;
		int t;
		while (b != 0)
		{
			t = b;
			b = a % b;
			a = t;
		}
		return a;
	}
	public static int lcm(int first , int second){
		return ((first*second)/gcd(first,second));
	}
}
