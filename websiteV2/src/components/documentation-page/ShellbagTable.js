import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import ShellbagTableImg from '../../assets/shellbag-table.png';

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
        overflow: 'auto',
        borderRadius: '0',
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
}

class ShellbagTable extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Shellbag Table</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The shellbag table displays common shellbag data with the purpose of being able to easily sort through shellbags based on their fields.
                </Typography>
                <img src={ShellbagTableImg} alt="shellbag-table" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    To sort by a particular field:
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
            </div>
        )
    }
}

export default withStyles(styles)(ShellbagTable);