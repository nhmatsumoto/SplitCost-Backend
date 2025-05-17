import { Link } from "react-router-dom"

const HousePage = () => {
    return (
        <>
            <Link to="/house/create">
                <button className="bg-[#00796B] text-white px-4 py-2 rounded-lg">
                    Create House
                </button>       
            </Link>
        </>
    )
}

export default HousePage;