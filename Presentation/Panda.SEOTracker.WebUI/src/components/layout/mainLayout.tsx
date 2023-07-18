import './layout.scss';
import { Button, Col, Grid, Layout, Menu, MenuProps, Row, Switch, theme } from 'antd';
import React, { useContext, useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Content, Header } from 'antd/es/layout/layout';
import Sider from 'antd/es/layout/Sider';
import { useClickAway } from 'ahooks';
import { AppContext } from '../contexts/appContext';
import { icon } from '@fortawesome/fontawesome-svg-core/import.macro';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Admin } from './navigationMenuItems';

const { useBreakpoint } = Grid;
const { useToken } = theme;

type Props = {
    children: React.ReactNode;
};

const MainLayout: React.FC<Props> = (props): JSX.Element => {
    const screens = useBreakpoint();
    const [collapsed, setCollapsed] = useState(true);
    const [collapseEnabled, setCollapseEnabled] = useState(true);
    const [currentKey, setCurrentKey] = useState('');
    const navigate = useNavigate();
    const sliderButtonRef = useRef(null);
    const sliderContainerRef = useRef(null);
    const { token } = useToken();
    const appContext = useContext(AppContext);

    useEffect(() => {
        setCollapseStates();
        setCurrentKeyFromUri();
    }, []);

    useEffect(() => setCurrentKeyFromUri(), [window.location.pathname]);

    useEffect(() => setCollapseStates(), [screens]);

    useClickAway(() => {
        if (collapseEnabled && !collapsed) {
            setCollapsed(true)
        }
    }, [sliderButtonRef, sliderContainerRef]);

    function setCollapseStates() {
        if (screens.lg || screens.xl || screens.xxl) {
            setCollapseEnabled(false);
            setCollapsed(false);
        } else {
            setCollapseEnabled(true);
            setCollapsed(true);
        }
    }
    function setCurrentKeyFromUri() {
        var navItem = Admin.find(x => window.location.pathname === x.path);
        if (navItem == undefined)
            navItem = Admin.find(x => window.location.pathname.startsWith(x.path));
        setHtmlTitle(navItem?.displayName ?? "Not Found");
        setCurrentKey(navItem?.displayName.toLocaleLowerCase() ?? "/notFound");
    }
    function navigateMenu(key: string) {
        const navItem = Admin.find(x => x.displayName.toLowerCase() == key);
        setHtmlTitle(navItem?.displayName ?? "Not Found");
        navigate(navItem?.path ?? "/notFound");
    }
    function setHtmlTitle(value: string) {
        document.title = `Panda ${value != "" ? `| ${value}` : ""}`;
    }

    const displayNoneStyle = {
        display: "none"
    }
    const onClick: MenuProps['onClick'] = (e) => {
        navigateMenu(e.key);
    };
    const logo =
        <div style={{ margin: "0.5rem", marginBottom: "0", display: "flex", flexDirection: "column", }}>
            <img alt="Logo"
                onClick={() => navigateMenu("Home")}
                className="logo"
                src={`/public/images/Logo-${appContext.isDarkTheme ? "Light" : "Dark"}.png`} />
            <span data-is-dark-theme={appContext.isDarkTheme}>
                SEO Tracker
            </span>
        </div>;
    return (
        <Layout className="panda-layout" style={{ height: "100vh", backgroundColor: token.colorBgBase }}>
            {!collapseEnabled ? <></> :
                <Row className="panda-logo-row"
                    style={{ backgroundColor: token.colorBgBase }}>
                    <Col>{logo}</Col>
                    <Col style={collapseEnabled ? { display: "flex", marginLeft: "2rem" } : displayNoneStyle}>
                        <Button ref={sliderButtonRef}
                            type="primary"
                            style={{ margin: "auto", border: ".2rem white" }}
                            onClick={() => setCollapsed(x => !x)}>
                            <FontAwesomeIcon icon={icon({ name: 'bars' })} style={!collapsed ? displayNoneStyle : { marginInlineStart: "0" }} />
                            <FontAwesomeIcon icon={icon({ name: 'xmarks-lines' })} style={collapsed ? displayNoneStyle : { marginInlineStart: "0" }} />
                        </Button>
                    </Col>
                </Row>
            }

            <Layout className="panda-layout">
                <Header ref={sliderContainerRef} className="panda-header"
                    style={{ backgroundColor: token.colorBgBase }}>
                    <Sider className="panda-slider"
                        trigger={null}
                        collapsedWidth={0}
                        collapsible
                        style={{ backgroundColor: token.colorBgBase }}
                        collapsed={collapsed}>
                        <div style={{ display: "flex", height: "100%", flexDirection: "column", paddingBottom: "1rem" }}>
                            {collapseEnabled ? <></> : logo}
                            <Menu mode="vertical"
                                style={{ backgroundColor: token.colorBgBase }}
                                rootClassName="panda-menu"
                                items={Admin.map(x => ({
                                    label: x.displayName,
                                    key: x.displayName.toLowerCase(),
                                    path: x.path,
                                    style: {
                                        color: token.colorTextBase,
                                    }
                                }))
                                }
                                onClick={onClick}
                                selectedKeys={[currentKey]} />

                            <div style={{ margin: "auto auto 0 auto" }}>
                                <span>Theme </span>
                                <Switch
                                    checkedChildren={<span>Dark </span>}
                                    unCheckedChildren={<span>Light </span>}
                                    checked={appContext.isDarkTheme}
                                    onChange={() => appContext.toggleTheme()}
                                />
                            </div>
                        </div>
                    </Sider>
                </Header>
                <Layout className="panda-layout">
                    <Content className="panda-content" data-is-dark-theme={appContext.isDarkTheme}>
                        {props.children}
                    </Content>
                </Layout>
            </Layout>
        </Layout >
    );
};

export default MainLayout;