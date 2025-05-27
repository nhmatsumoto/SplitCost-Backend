import { Link } from "react-router-dom"
import { useResidenceStore } from "../../store/residenceStore";

const HousePage = () => {

    const houseInfo = useResidenceStore(state => state.residence);

    return (
        <>
            <Link to="/house/create">
                <button className="bg-[#00796B] text-white px-4 py-2 rounded-lg">
                    Create House
                </button>       
            </Link>

            {
                JSON.stringify(houseInfo)
            }
        </>
    )
}

export default HousePage;