import { icon } from '@fortawesome/fontawesome-svg-core/import.macro';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Button, Col, InputNumber, Modal, Row, Space, Spin, Tag } from 'antd';
import React, { useRef, useState } from 'react';
import { CreateTrackedUrlDto, TrackedUrlClient } from '../../../components/appHttpClient';
import InputHeaderText from '../../../components/display/inputHeaderText';
import InputErrorText from '../../../components/display/inputErrorText';
import Input from 'antd/es/input/Input';
import { useValidationErrors } from '../../../components/customHooks/useValidationErrors';
import ShowValidationErrosModal from '../../../components/modals/ShowValidationErrosModal';

interface IProps {
    onCreated: () => void
}
const CreateTrackedUrlModal: React.FC<IProps> = (props) => {
    const isCreating = useRef(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [searchTerm, setSearchTerm] = useState("");
    const [apiClient] = useState(new TrackedUrlClient());
    const [request, setRequest] = useState(new CreateTrackedUrlDto());
    const validation = useValidationErrors();

    function onCreate() {
        validation.set([])
        isCreating.current = true;
        apiClient.create(request)
            .then((x) => {
                props.onCreated()
                onCancel()
            }).catch((ex) => {
                validation.set(ex)
            }).finally(() => {
                isCreating.current = false;
            });
    }
    function onCancel() {
        isCreating.current = false
        validation.set([])
        setRequest(new CreateTrackedUrlDto)
        setIsModalOpen(false)
        setSearchTerm("")
    }

    function onUrlChanged(value: string) {
        setRequest(x => {
            x.url = value
            return JSON.parse(JSON.stringify(x))
        })
    }
    function onTotalResultToCheckChanged(value: number | null) {
        setRequest(x => {
            x.totalResultsToCheck = value!
            return JSON.parse(JSON.stringify(x))
        })
    }
    function onAddSearchTerm() {
        const newSearchTerm = searchTerm.trim();

        if (newSearchTerm != "" && !request?.searchTerms?.includes(newSearchTerm)) {
            setRequest(x => {
                if (x.searchTerms == null)
                    x.searchTerms = []
                x.searchTerms.push(newSearchTerm)
                return JSON.parse(JSON.stringify(x))
            })
            setSearchTerm("")
        }
    }
    function onDeleteSearchTerm(value: string) {
        if (request.searchTerms.includes(value))
            setRequest(x => {
                x.searchTerms = x.searchTerms.filter(x => x != value);
                return JSON.parse(JSON.stringify(x))
            })
    }
    return (<>
        <ShowValidationErrosModal validationErrors={validation.errors} filter={x => ["", "Id"].includes(x.propertyName)} />
        <Button size="large" type="primary" onClick={() => setIsModalOpen(true)}>
            <FontAwesomeIcon icon={icon({ name: 'plus' })} />
        </Button>
        <Modal title="New Tracked Url"
            open={isModalOpen}
            onOk={onCreate}
            onCancel={onCancel}
            destroyOnClose={true}
            okText={(isCreating.current ? <Spin /> : <>Submit</>)}>
            <Row>
                <Col style={{ marginBottom: "1rem" }} span={24}>
                    <InputHeaderText Title="Url" />
                    <Input placeholder="Url to track"
                        value={request.url}
                        suffix={<FontAwesomeIcon icon={icon({ name: 'globe' })} />}
                        onChange={(x) => onUrlChanged(x.target.value)} />
                    <InputErrorText propertyNames={["Url"]}
                        validationErrors={validation.errors} />
                </Col>
                <Col style={{ marginBottom: "1rem" }} span={24}>
                    <InputHeaderText Title="Total Results To Check" />
                    <InputNumber placeholder="Total Results To Check"
                        min={1}
                        style={{ width: "100%" }}
                        value={request.totalResultsToCheck}
                        onChange={(x) => onTotalResultToCheckChanged(x)} />
                    <InputErrorText propertyNames={["TotalResultsToCheck"]}
                        validationErrors={validation.errors} />
                </Col>
                <Col style={{ marginBottom: "1rem" }} span={24}>
                    <InputHeaderText Title="Search Terms" />
                    <Input placeholder="Add Search Term"
                        value={searchTerm}
                        suffix={<FontAwesomeIcon icon={icon({ name: 'plus' })} onClick={onAddSearchTerm} />}
                        onChange={(x) => setSearchTerm(x.target.value)}
                        onPressEnter={onAddSearchTerm} />
                    <InputErrorText propertyNames={["SearchTerms"]}
                        validationErrors={validation.errors} />

                    <Space size={[0, 'large']} wrap>
                        {request?.searchTerms?.map(
                            x => <Tag key={x}
                                bordered={false}
                                closable
                                onClose={() => onDeleteSearchTerm(x)}
                                children={x} />)}
                    </Space>
                </Col>
            </Row>
        </Modal>
    </>);
};

export default CreateTrackedUrlModal;