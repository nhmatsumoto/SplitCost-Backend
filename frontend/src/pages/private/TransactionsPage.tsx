import { Link } from "react-router-dom";

const TransactionsPage = () => {
    return (
         <>
            <h1>Transactions</h1>
            <p>Nesta página a ideia é ter um lugar para registrar entradas e despesas (Incomes and Expenses)</p>
            <Link to="/">Go to Home</Link>
        </>
    )
}

export default TransactionsPage;