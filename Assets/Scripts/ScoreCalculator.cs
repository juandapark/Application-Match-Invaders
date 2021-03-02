/*********************************
 * Created by: David Giraldo
 * Date Created: 02/02/2021
 * Last Updated by: David Giraldo
 * Last Updated: 02/02/2021
 *
 * Files handles the Calculation of the score.
 * ******************************************/

public class ScoreCalculator 
{
    private  const int SCORE_MULIPLIER = 10; // the score multiplier fot the fibonacci formula.

    /// <summary>
    /// Passes the number of ships destroyed and returns the score.
    /// </summary>
    /// <param name="numberOfShips"></param>
    /// <returns></returns>
    public static uint ReturnScore(uint numberOfShips)
    {
        uint score = numberOfShips * ((FibonacciFormula(numberOfShips + 1)) * SCORE_MULIPLIER);
        return score;
    }

    /// <summary>
    /// Calculate score based on Fibonacci Formula.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private static uint FibonacciFormula(uint n)
    {
        uint firstnumber = 0, secondnumber = 1, result = 0;

        if (n == 0) return 0;
        if (n == 1) return 1;

        for (uint i = 2; i <= n; i++)
        {
            result = firstnumber + secondnumber;
            firstnumber = secondnumber;
            secondnumber = result;
        }

        return result;
    }
}
