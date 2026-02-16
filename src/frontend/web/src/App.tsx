import { useState, useEffect } from 'react'
import './App.css'

async function getJson(path: string) {
  const res = await fetch(path);
  if (!res.ok) throw new Error(await res.text());
  return res.json();
}

function App() {
  const [owners, setOwners] = useState<any[]>([]);

  useEffect(() => {
    getJson('http://localhost:8080/api/owners-service/owners').then(setOwners);
  }, []);

  return (
    <>
      <h1>Owners</h1>
      <ul>
        {owners.map((owner) => (
          <li key={owner.id}>{owner.lastName} {owner.firstName}</li>
        ))}
      </ul>

    </>
  )
}

export default App
