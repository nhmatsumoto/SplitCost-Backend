import { Link } from "react-router-dom";

const ExpensesPage = () => {
    return (
         <>
            <Link to="/expense/create">
                <button className="bg-[#00796B] text-white px-4 py-2 rounded-lg">
                    Create Expense
                </button>       
            </Link>
        </>
    )
}

export default ExpensesPage;