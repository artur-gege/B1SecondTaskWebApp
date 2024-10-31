import { useState, useEffect } from "react";

function SFiles({setKey, setActive }) {

    const [files, setFiles] = useState([])
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const getFiles = async () => {
        setError(null)
        setIsLoading(true)
        try {
            let response = await fetch('https://localhost:7284/api/File')
            response = await response.json()
            console.log(response)
            setFiles(response)
        } catch (error) {
            console.error('fail', error);
            setError(error.message);
            setIsLoading(false);
        } finally {
            setIsLoading(false)
        }
    }

    useEffect(() => {
        getFiles()
    }, [])


    return (
        <section>
            {isLoading && <h4> 'Loading...'</h4>}
            {files.map((e, i) => (
                <article onClick={() => {
                    setActive(2)
                    setKey(e.id)
                }} key={e.id}>
                    <h4>{i + 1}. {e.fileName}</h4>
                    <h4>{e.uploadedDate.replace("T", ' ').slice(0, 19)}</h4>
                </article>)
            )}
        </section>
  );
}

export default SFiles;