import React, { useState } from 'react';
import { useTranslator } from './contexts/translationContext';
import './App.css';

const App = () => {
  const { translations, error, loading, translationsCount, translate } =
    useTranslator();
  const [input, setInput] = useState(translations?.input);
  const onSubmit = async (e) => {
    e.preventDefault();
    await translate(input);
  };
  return (
    <div className='App'>
      <form className='form' onSubmit={onSubmit}>
        <button className='button' disabled={loading || !input}>
          Translate
        </button>
        <div className='row'>
          <h2 className='language'>English</h2>
          <input
            type='input'
            onFocus={() => setInput('')}
            onChange={(e) => {
              setInput(e.target.value);
            }}
            value={input}
          />
        </div>
        <div className='row'>
          <h2 className='language'>French</h2>
          <input disabled value={translations?.french || ''} />
        </div>
        <div className='row'>
          <h2 className='language'>Spanish</h2>
          <input disabled value={translations?.spanish || ''} />
        </div>
      </form>
      <h4>{`Count of translations: ${translationsCount}`}</h4>
      {error && <h4 className='error'>{`Error: ${error}`}</h4>}
    </div>
  );
};

export default App;
