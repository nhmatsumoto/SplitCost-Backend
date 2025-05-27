import { Link } from "react-router-dom";
import { useResidenceStore } from "../../store/residenceStore";

const ResidencePage = () => {
    const residence = useResidenceStore(state => state.residence);
    const address = residence?.address;

    return (
        <>
            {residence ? (
                <div className="bg-white p-4 rounded-lg shadow-md">
                    <h2 className="text-xl font-semibold mb-2">House Details</h2>
                    <p><strong>Residence Name:</strong> {residence.name}</p>
                    {address ? (
                        <p>
                            <strong>Address:</strong>{" "}
                            {address.street}, {address.number}, {address.city}, {address.prefecture}, {address.country}, {address.postalCode}
                        </p>
                    ) : (
                        <p><strong>Address:</strong> Not available</p>
                    )}
                </div>
            ) : (
                <>
                    <div className="bg-white p-4 rounded-lg shadow-md">
                        <h2 className="text-xl font-semibold mb-2">No House Found</h2>
                        <p>Please create a house to view details.</p>
                    </div>

                    <Link to="/residence/create">
                        <button className="bg-[#00796B] text-white px-4 py-2 rounded-lg mt-2">
                            Create House
                        </button>
                    </Link>
                </>
            )}
        </>
    );
};

export default ResidencePage;
