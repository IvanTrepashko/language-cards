import React, {useState} from 'react';

function LanguageCard({languageCard}) {
    const [guessPhrase, setGuessPhrase] = useState("")
    const [result, setResult] = useState("")
    const [isCorrect, setIsCorrect] = useState(false)

    const handleCheck = () => {
        if (guessPhrase.toLowerCase() === languageCard.translateToPhrase.toLowerCase()) {
            setResult("Correct!")
            setIsCorrect(true)
        }
        else {
            setResult("Incorrect ☹")
            setIsCorrect(false)
        }
    }

    return (
        <div className="flex justify-center">
            <div className="flex flex-col bg-white justify-center basis-1/4 border-2 rounded-lg  max-w-fit min-w-fit">
                <div className="flex flex-row space-x-10 m-5">
                    <div className="flex-col basis-1/2 text-center">
                        <span>{languageCard.translateFromLanguage}</span>
                        <br/>
                        <input className="mt-3 shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" value={languageCard.translateFromPhrase} disabled={true}/>
                    </div>
                    <div className="flex-col basis-1/2 text-center">
                        <span>{languageCard.translateToLanguage}</span>
                        <br/>
                        <input className="mt-3 shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" value={guessPhrase} onChange={(e) => setGuessPhrase(e.target.value) }/>
                    </div>
                </div>
                <div className="flex flex-col text-center">
                    <div className="mb-3">
                        <button className="bg-transparent hover:bg-blue-500 text-blue-700 font-semibold hover:text-white py-2 px-4 border border-blue-500 hover:border-transparent rounded" onClick={handleCheck}>Check</button>
                        <p className={ "text-xl mt-5 " + (isCorrect ? "text-green-500" : "text-red-500") }>{result}</p>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default LanguageCard;