import { List, Modal, Typography } from 'antd';
import React, { useEffect, useState } from 'react';
import { ValidationError } from '../appHttpClient';
import { icon } from '@fortawesome/fontawesome-svg-core/import.macro';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

interface IProps {
    validationErrors: ValidationError[]
    filter?: (value: ValidationError, index: number, array: ValidationError[]) => unknown, thisArg?: any
}
const ShowValidationErrosModal: React.FC<IProps> = (props) => {
    const [isModalOpen, setIsModalOpen] = useState(false);

    useEffect(() => {
        setIsModalOpen(getDataSource().length > 0)
    }, [props.validationErrors])

    const modalTitle = (
        <Typography.Title
            style={{ margin: 0 }}
            level={4}>
            <FontAwesomeIcon
                size={"1x"}
                color="red"
                style={{ marginRight: ".5rem" }}
                icon={icon({ name: 'xmark-circle' })} />
            Errors
        </Typography.Title>)

    function getDataSource() {
        return props.filter == null ? props.validationErrors : props.validationErrors.filter(props.filter)
    }
    return (<>
        <Modal open={isModalOpen}
            title={modalTitle}
            footer={<></>}
            onCancel={() => { setIsModalOpen(false) }}>
            <List dataSource={getDataSource()}
                bordered
                renderItem={(item) => (
                    <List.Item style={{ width: "100%" }}>
                        {item.message}
                    </List.Item>
                )}
            />
        </Modal>
    </>);
};

export default ShowValidationErrosModal;