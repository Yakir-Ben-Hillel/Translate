import { useEffect, createContext, useContext, useMemo, useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import axios from 'axios';

const localStorageTokenKey = 'translationApiToken';
const AuthContext = createContext();
const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(
    localStorage.getItem(localStorageTokenKey)
  );

  // Function to refresh the token
  const refreshToken = async () => {
    try {
      const response = await axios.get(
        `${process.env.REACT_APP_PIM_TRANSLATION_API_ENDPOINT}/Auth/HandShake`
      );
      setToken(response.data.data.token);
      localStorage.setItem(localStorageTokenKey, response.data.data.token);
    } catch (error) {
      console.error('Failed to refresh token', error);
    }
  };

  // Function to set a timeout to refresh the token 1 minute before it expires
  const setTokenTimeout = (token) => {
    let timeoutDuration = 1;
    if (token) {
      const decodedToken = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      const expiryTime = decodedToken.exp;
      timeoutDuration = (expiryTime - currentTime - 60) * 1000; // Convert to milliseconds and subtract 1 minute
    }
    setTimeout(refreshToken, timeoutDuration);
  };

  // Call setTokenTimeout on component mount and every time the token changes
  useEffect(() => {
    if (token)
      axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;
    setTokenTimeout(token);
  }, [token]);

  const contextValue = useMemo(() => token, [token]);
  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};
export const useAuth = () => {
  return useContext(AuthContext);
};
export default AuthProvider;
