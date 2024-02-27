import { createContext, useContext, useMemo, useState } from 'react';
import axios from 'axios';

const TranslationContext = createContext();
const TranslationProvider = ({ children }) => {
  const [translations, setTranslations] = useState(
    JSON.parse(localStorage.getItem('translations'))
  );
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [translationsCount, setTranslationsCount] = useState(0);

  const translate = async (text) => {
    setLoading(true);
    try {
      setError(null);
      var { data } = await axios.get(
        `${process.env.REACT_APP_PIM_TRANSLATION_API_ENDPOINT}/Translation/Translate/${text}`
      );
      if (!data.success) {
        setError(data.message);
        return;
      }
      const payload = {
        input: data.data.input,
        french: data.data.frenchTranslation,
        spanish: data.data.spanishTranslation,
      };
      localStorage.setItem('translations', JSON.stringify(payload));
      setTranslations(payload);
      setTranslationsCount((prev) => prev + 1);
    } catch (e) {
      setError('Internal Server Error');
    } finally {
      setLoading(false);
    }
  };
  const contextValue = useMemo(() => {
    return {
      translations,
      error,
      loading,
      translationsCount,
      translate,
    };
  }, [translations, error, loading, translationsCount]);
  return (
    <TranslationContext.Provider value={contextValue}>
      {children}
    </TranslationContext.Provider>
  );
};
export const useTranslator = () => {
  return useContext(TranslationContext);
};
export default TranslationProvider;
