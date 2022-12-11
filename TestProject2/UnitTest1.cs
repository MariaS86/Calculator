using OPZcalc;

namespace TestProject2
{
    [TestClass]
    public class OPZcalcTest
    {
        //проверка преобразования строки в постфиксную запись(обр польскую запись)
        [TestMethod]
        public void GetExpression_Test()
        {
            calc res = new calc();
            Assert.AreEqual("1 1 + ", res.GetExpression("1+1"));
            Assert.AreEqual("1 2 * 1 + ", res.GetExpression("1*2+1"));
            Assert.AreEqual("4 2 / 0,4 * 1 + ", res.GetExpression("4/2*0,4+1"));
            Assert.AreEqual("4 2 / 0,4 1 + * ", res.GetExpression("(4/2)*(0,4+1)"));
        }
        //проверка вычисления выражения из постфиксной записи
        [TestMethod]
        public void Counting_Test()
        {
            calc res = new calc();
            Assert.AreEqual(2, res.Counting("1 1 + "));
            Assert.AreEqual(3, res.Counting("1 2 * 1 + "));
            Assert.AreEqual(1.8, res.Counting("4 2 / 0,4 * 1 + "));
            Assert.AreEqual(2.8, res.Counting("4 2 / 0,4 1 + * "));

        }
        //проверяемый символ - разделитель ("пробел" или "равно")
        [TestMethod]
        public void IsDelimeter_Test()
        {
            calc res = new calc();
            Assert.AreEqual(true, res.IsDelimeter('='));
            Assert.AreEqual(true, res.IsDelimeter(' '));
            Assert.AreEqual(false, res.IsDelimeter('/'));
        }
        //проверка проверяемый символ - оператор
        [TestMethod]
        public void IsOperator_Test()
        {
            calc res = new calc();
            Assert.AreEqual(true, res.IsOperator('+'));
            Assert.AreEqual(true, res.IsOperator('-'));
            Assert.AreEqual(true, res.IsOperator('/'));
            Assert.AreEqual(true, res.IsOperator('*'));
            Assert.AreEqual(true, res.IsOperator('^'));
            Assert.AreEqual(true, res.IsOperator('('));
            Assert.AreEqual(true, res.IsOperator(')'));
            Assert.AreEqual(false, res.IsOperator('1'));

        }
        //проверка возврата приоритета оператора
        [TestMethod]
        public void GetPriority_Test()
        {
            calc res = new calc();
            Assert.AreEqual(0, res.GetPriority('('));
            Assert.AreEqual(1, res.GetPriority(')'));
            Assert.AreEqual(2, res.GetPriority('+'));
            Assert.AreEqual(3, res.GetPriority('-'));
            Assert.AreEqual(4, res.GetPriority('*'));
            Assert.AreEqual(4, res.GetPriority('/'));
            Assert.AreEqual(5, res.GetPriority('^'));
            Assert.AreEqual(6, res.GetPriority('='));

        }
    }
}