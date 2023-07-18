import { icon } from '@fortawesome/fontawesome-svg-core/import.macro';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Button, List, Table, Typography } from 'antd';
import React, { useRef, useState } from 'react';
import { SearchTermHistoryDto, TrackedUrlClient, TrackedUrlDto } from '../../../components/appHttpClient';
import Modal from 'antd/es/modal/Modal';
import ShowValidationErrosModal from '../../../components/modals/ShowValidationErrosModal';
import { useValidationErrors } from '../../../components/customHooks/useValidationErrors';
import Column from 'antd/es/table/Column';

interface IProps {
    trackedUrlId: string
}
const LatestTrackingModal: React.FC<IProps> = (props) => {
    const isSearching = useRef(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [trackedUrl, setTrackedUrl] = useState<TrackedUrlDto | undefined>();
    const [apiClient] = useState(new TrackedUrlClient());
    const validation = useValidationErrors();
    function onLatestTracking() {
        isSearching.current = true;
        apiClient.getLatestTrackingInformation(props.trackedUrlId)
            .then((x) => {
                setTrackedUrl(x);
                setIsModalOpen(true);
            }).catch((ex) => {
                validation.set(ex)
                setIsModalOpen(false);
            }).finally(() => {
                isSearching.current = false;
            });
    }
    function onCancel() {
        isSearching.current = false;
        setIsModalOpen(false);
    }

    return (<>
        <ShowValidationErrosModal validationErrors={validation.errors} />
        <Button icon={<FontAwesomeIcon icon={icon({ name: 'magnifying-glass-dollar' })} />}
            style={{ backgroundColor: "rgb(170, 149, 20)" }}
            onClick={onLatestTracking}
            type="primary"
            shape="circle"
            size="large"

        />
        <Modal title="Latest Tracking Information"
            open={isModalOpen}
            footer={<></>}
            onCancel={onCancel}
            destroyOnClose={true}>
            {trackedUrl?.url}
            <Table style={{ marginTop: "1rem" }}
                dataSource={trackedUrl?.searchTerms}
                loading={isSearching.current}
                rowKey="id"
                expandable={{
                    expandedRowRender: (record) => {
                        return (<>
                            <List
                                itemLayout="horizontal"
                                dataSource={record.history}
                                renderItem={(item, index) => (
                                    <List.Item key={index}>
                                        <List.Item.Meta
                                            title={`${item.searchEngineUsed == 1 ? "Google" : "N/A"} - ${item.date.toDateString()} `}
                                            description={
                                                <div key={index}>
                                                    <Typography.Text>Count: {item.positions.length}</Typography.Text>
                                                    <br />
                                                    <Typography.Text>Positions:</Typography.Text>
                                                    <br />
                                                    {`${(item.positions.map((x) => x.toString() + " "))}`}
                                                </div>}
                                        />
                                    </List.Item>
                                )}
                            />
                        </>)
                    },
                }}
            >
                <Column title="Term" dataIndex="term" />
                <Column title="History" dataIndex="history"
                    render={(value: SearchTermHistoryDto[], _, index) =>
                        <>
                            {value.length}
                        </>
                    } />
            </Table>
        </Modal>
    </>);
};

export default LatestTrackingModal;