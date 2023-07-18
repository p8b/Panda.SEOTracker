import { Button, Popconfirm, Space, Table, TablePaginationConfig, Tag, theme } from 'antd';
import React, { forwardRef, useEffect, useImperativeHandle, useRef, useState } from 'react';
import { SearchTermDto, SortByDirection, TrackedUrlClient, TrackedUrlDto } from '../../../components/appHttpClient';
import { useValidationErrors } from '../../../components/customHooks/useValidationErrors';
import Column from 'antd/es/table/Column';
import ShowValidationErrosModal from '../../../components/modals/ShowValidationErrosModal';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { icon } from '@fortawesome/fontawesome-svg-core/import.macro';
import LatestTrackingModal from './latestTrackingModal';

interface IProps {
    searchValue: string
}
export interface ITrackedUrlTable {
    reloadData: () => void
}
const TrackedUrlTable = forwardRef<ITrackedUrlTable, IProps>((props, ref) => {
    const isLoading = useRef(false);
    const isDeleting = useRef("");
    const [pagination, setPagination] = useState<TablePaginationConfig>(new Object());
    const [apiClient] = useState(new TrackedUrlClient());
    const [data, setData] = useState<TrackedUrlDto[]>();
    const validation = useValidationErrors();
    const { token } = theme.useToken()

    useImperativeHandle(ref, (): ITrackedUrlTable => {
        return {
            reloadData() {
                onSearch(1)
            }
        } as ITrackedUrlTable
    })
    useEffect(() => {
        onSearch()
    }, [])

    useEffect(() => {
        onSearch()
    }, [props.searchValue])

    function onSearch(pageNumber: number = pagination?.current ?? 1, PageSize: number = pagination?.pageSize ?? 10) {
        isLoading.current = true;
        apiClient.searchTrackedUrls(
            props.searchValue,
            null,
            SortByDirection.Ascending,
            pageNumber,
            PageSize)
            .then((x) => {
                if (x != null) {
                    setData(x.data);
                    setPagination(p => {
                        p.current = x.currentPage;
                        p.pageSize = x.pageSize;
                        p.total = x.totalCount;
                        p.onChange = onSearch;
                        return p;
                    })
                }
            }).catch((ex) => {
                validation.set(ex)
            }).finally(() => {
                isLoading.current = false;
            });
    }

    function onDelete(id: string) {
        isDeleting.current = id
        apiClient.delete(id)
            .then(() => {
                onSearch()
            }).catch((ex) => {
                validation.set(ex)
            }).finally(() => {
                isDeleting.current = "";
            });
    }

    return (<>
        <ShowValidationErrosModal validationErrors={validation.errors} />
        <Table dataSource={data} pagination={pagination} loading={isLoading.current} rowKey="id">
            <Column title="Url" dataIndex="url" />
            <Column title="Total Results To Check" dataIndex="totalResultsToCheck" />
            <Column title="Search Terms" dataIndex="searchTerms" align="center"
                render={(value: SearchTermDto[], item: TrackedUrlDto, index) => {
                    return (

                        <Popconfirm
                            title="Search Terms"
                            showCancel={false}
                            description={
                                <div style={{ margin: "1rem 0 1rem 0" }} key={index} >
                                    <Space size={[0, 'large']} wrap>
                                        {value?.map(x => <Tag key={x.id} bordered={false} children={x.term} />)}
                                    </Space>
                                </div>
                            }
                            icon={<FontAwesomeIcon style={{ marginRight: ".5rem" }} icon={icon({ name: 'info-circle' })} />}>
                            <Button icon={<FontAwesomeIcon
                                icon={icon({ name: 'info' })} />}
                                type="primary"
                                style={{ backgroundColor: token['blue-5'] }}
                                size="large"
                                children={<>({value.length})</>}
                            />
                        </Popconfirm>
                    )
                }} />
            <Column align="center"
                render={(_, x: TrackedUrlDto, index) => {
                    return (
                        <Space key={index} size={[5, 'large']} wrap >

                            <LatestTrackingModal trackedUrlId={x.id} />

                            <Popconfirm
                                title="Delete"
                                description={`Are you sure to delete ${x.url}?`}
                                onConfirm={() => onDelete(x.id)}
                                okText="Yes"
                                cancelText="No">
                                <Button icon={<FontAwesomeIcon
                                    icon={icon({ name: 'x' })} />}
                                    loading={isDeleting.current == x.id}
                                    type="primary"
                                    style={{ backgroundColor: token['red-5'] }}
                                    shape="circle"
                                    size="large" />
                            </Popconfirm>
                        </Space>
                    )
                }} />
        </Table>
    </>);
});

export default TrackedUrlTable;