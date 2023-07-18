import React from 'react';
import { ConfigProvider, theme, ThemeConfig } from 'antd';

type Props = {
    children: React.ReactNode;
    isDarkTheme: boolean
};

const AppThemeProvider: React.FC<Props> = (props): JSX.Element => {
    const themeConfiguration: ThemeConfig =
    {
        token: {
            "colorPrimary": props.isDarkTheme ? "rgb(112, 169, 9)" : "rgb(62, 93, 5)",
            "colorSuccess": "#9ab865",
            "colorError": "#ff0000",
            "colorWarning": "#ff850a",
            "colorBgBase": props.isDarkTheme ? "rgba(32, 33, 36,.55)" : "rgba(240, 240, 240,.8)",
            "colorBgLayout": props.isDarkTheme ? "rgba(32, 33, 36,.55)" : "rgba(240, 240, 240,.4)",
        },
        algorithm: props.isDarkTheme ? theme.darkAlgorithm : theme.defaultAlgorithm,
    };
    return (
        <ConfigProvider theme={themeConfiguration} >
            {props.children}
        </ConfigProvider>
    );
};

export default AppThemeProvider;