import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";
import EventTableImg from '../../assets/shellbag-event-table.png';
import TableFilter from '../../assets/table-filter.png';

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
        color: '#33A1FD',
        textAlign: 'center',
    },
    text: {
        textAlign: 'center',
        padding: '1%',
    },
    video: {
        maxHeight: '360px',
        maxWidth: '640px',
        minHeight: '360px',
        minWidth: '300px',
        height: '50%',
        width: '50%',
        margin: '10px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        backgroundColor: '#424242',
    },
}


class EventTable extends React.Component {

    render() {
        return(
            <div className={this.props.classes.content}>
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=Yxewd4y6Y7Y"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The shellbag event table displays the interpreted shellbag events taken from the parsed shellbags. More information on shellbag
                    events can be found in the shellbag event tab from the menu.
                </Typography>
                <img src={EventTableImg} alt="event-table" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>To sort by a particular field:</b>
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. Click the column header and the fields will sort in ascending order.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. Click the column header again and the fields will sort in descending order.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. Click on a different column header to sort by a different field.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    When a user filters by shellbag event type on the timeline, the table will update to highlight only events of that type.
                </Typography>
                <img src={TableFilter} alt='table-filter'/>
            </div>
        );
    }
}

export default withStyles(styles)(EventTable);