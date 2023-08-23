import Axios from 'axios'
import {useQuery} from "react-query";
import LanguageCard from "../../components/cards/LanguageCard";
import {useState} from "react";

function Home() {
    const [languageCardData, setLanguageCardData] = useState({})
    const { isLoading, data } = useQuery('language-card', async () => {
        const {data} = await Axios.get("https://localhost:44395/language-card/random")
        setLanguageCardData(data)
        return data
    })

    const handleGetRandomCard = async () => {
        const {data} = await Axios.get("https://localhost:44395/language-card/random")
        setLanguageCardData(data)
    }

    if (isLoading) {
        return <h1>Loading....</h1>
    }

    return (
        <div className="text-center">
            <p>Guess a random card</p>
            <div className="mt-3">{data && <LanguageCard key={languageCardData.id} languageCard={languageCardData} />}</div>
            <button onClick={handleGetRandomCard} className="mt-3 bg-white hover:bg-gray-100 text-gray-800 font-semibold py-2 px-4 border border-gray-400 rounded shadow">
                One more!
            </button>
        </div>
    );
}

export default Home;