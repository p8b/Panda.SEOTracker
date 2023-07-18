import { Col, Row } from 'antd';
import React, { useRef, useState } from 'react';
import Title from 'antd/es/typography/Title';
import CreateTrackUrlModal from './components/createTrackedUrlModal';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { icon } from '@fortawesome/fontawesome-svg-core/import.macro';
import Search from 'antd/es/input/Search';
import TrackedUrlTable, { ITrackedUrlTable } from './components/trackedUrlTable';

const TrackUrlPage: React.FC = () => {
    const tableRef = useRef<ITrackedUrlTable | null>(null)
    const [seachValue, setSearchValue] = useState("");
    function onSearch(value: string) {
        setSearchValue(value);
    }

    return <>
        <Row>
            <Col span={24}>
                <Title style={{ marginTop: "0" }}>Track Url</Title>
            </Col>
            <Col span={24} >
                <Row>
                    <Col flex="auto" >
                        <Search
                            placeholder="Search Url"
                            size="large"
                            enterButton={<FontAwesomeIcon icon={icon({ name: 'magnifying-glass' })} />}
                            onSearch={onSearch}
                            allowClear
                        />
                    </Col>
                    <Col style={{ marginLeft: "1rem" }}>
                        <CreateTrackUrlModal onCreated={() => { tableRef.current?.reloadData() }} />
                    </Col>
                </Row>
            </Col>
            <Col span={24} style={{ marginTop: "1rem" }}>
                <TrackedUrlTable ref={tableRef} searchValue={seachValue} />
            </Col>
        </Row>
    </>;
};

export default TrackUrlPage;