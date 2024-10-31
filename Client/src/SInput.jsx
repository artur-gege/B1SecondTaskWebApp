import { useState, useEffect } from "react";

function SInput() {

    const [selectedFile, setSelectedFile] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded]  = useState(false)

    const handleFileChange = (event) => {
        setIsLoaded(false)
        setSelectedFile(event.target.files[0]);
    };

    useEffect(() => {
        setIsLoaded(false)
    }, [])

    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setError(null);

        const formData = new FormData();
        formData.append('file', selectedFile);

        try {

            const response = await fetch('https://localhost:7284/api/File', {
                method: 'POST',
                body: formData,
            })

            console.log(response)

            // Обрабатываем ответ от сервера
            console.log('loaded:', response.data);
            setIsLoading(false);
            setIsLoaded(true)
        } catch (error) {
            console.error('fail', error);
            setError(error.message);
            setIsLoading(false);
        }
    };


    return (
        <form onSubmit={handleSubmit}>
            <input type="file" onChange={handleFileChange} />
            <button className='loadButton' type="submit" disabled={isLoading}>
                {isLoading ? 'Loading...' : 'Load'}
            </button>
            {error && <p className="error">{error}</p>}
            {isLoaded && <h4>Loaded</h4>}
        </form>
  );
}

export default SInput;