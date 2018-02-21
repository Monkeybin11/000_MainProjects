using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regression
{
    public interface RegressionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceX">x 목록</param>
        /// <param name="sourceY">y 목록</param>
        /// <param name="function">피팅해야할 함수</param>
        /// <param name="initialFactor">피팅해야할 변수들의 초기값</param>
        /// <param name="fittedY">피팅한 결과 y 목록</param>
        /// <param name="fittedFactor">피팅 된 변수들</param>
        /// <param name="exitcondition">피팅을 빨리 끝내는 탈출조건</param>
        /// <returns></returns>
        bool DoRegression(double[] sourceX, double[] sourceY, Func<double[], double, double> function, double[] initialFactor, out double[] fittedY, out double[] fittedFactor, double exitcondition = 0);
    }
}
