namespace PaymentSystem
{
    public partial class Form1 : Form
    {
        PaymentManager paymentManager;
        public string paymentType = "";
        Dictionary<string, string> paymentNames;
        public Form1()
        {
            InitializeComponent();
            paymentManager = new PaymentManager();
            paymentManager.CheckConfigFile();
            paymentNames = paymentManager.GetPaymentTypes();
            comboBox1.Items.AddRange(paymentNames.Keys.Cast<object>().ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentType))
                {
                    MessageBox.Show("Please select a payment type!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                label3.Text = paymentManager.pay(long.Parse(textBox1.Text));
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            paymentType = comboBox1.SelectedItem.ToString();
            paymentManager.SetPaymentType(paymentNames[paymentType]);

        }
    }
}
