import React, { createContext, useEffect, useState } from "react"
import AppThemeProvider from "../layout/appThemeProvider"

interface IAppContext {
    isDarkTheme: boolean,
    toggleTheme: () => void
}

export const AppContext = createContext({
    isDarkTheme: true,
    toggleTheme: () => { }
} as IAppContext)

const AppContextProvider = ({ children }: Props): JSX.Element => {
    const isDarkThemeKey = "isDarkTheme"
    const [isDarkTheme, setIsDarkTheme] = useState(true)

    useEffect(() => {
        var theme = localStorage.getItem(isDarkThemeKey)
        if (theme != null)
            setIsDarkTheme(JSON.parse(theme) as boolean)
    }, [])

    useEffect(() => {
        localStorage.setItem(isDarkThemeKey, JSON.stringify(isDarkTheme))
    }, [isDarkTheme])

    function toggleTheme() {
        setIsDarkTheme(x => x = !x)
    }

    return (
        <AppContext.Provider value={{
            isDarkTheme,
            toggleTheme,
        }}>
            <AppThemeProvider isDarkTheme={isDarkTheme}>
                {children}
            </AppThemeProvider>
        </AppContext.Provider>
    )
}
export default AppContextProvider

type Props = {
    children: React.ReactNode
}