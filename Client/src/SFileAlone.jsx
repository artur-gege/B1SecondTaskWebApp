import {useState, useEffect } from 'react'
import Table from './Table';

function SFile({ cardKey, setActive }) {
    const [files, setFiles] = useState([])
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const getFile = async () => {
        setError(null)
        setIsLoading(true)
        try {
            let response = await fetch(`https://localhost:7284/api/File/${cardKey}`)
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
        getFile()
    }, [])
  return (
      <section>
          <button onClick={() => setActive(0)}>Close</button>
          {isLoading && <h4>loading...</h4>}

          {isLoading || <Table arr={files} />}
      </section>
  );
}

export default SFile;