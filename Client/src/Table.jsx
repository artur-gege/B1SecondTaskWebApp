import {useState } from 'react'

function Table({ arr }) {
    let r = '0'
    return (
        <section className="tableCont"> 
            <h2>Turnover sheet for balance sheet accounts</h2>
            <h4>for the period from 01/01/2016 to 12/31/2016</h4>
            <h4>by bank</h4>
            <table>
                <tr className='fatRow'>
                    <td rowSpan={2}>B/A</td>
                    <td colSpan={2}>INGOING BALANCE</td>
                    <td colSpan={2}>REVERSES</td>
                    <td colSpan={2}>OUTGOING BALANCE</td>
                </tr>
                <tr className='fatRow'>
                    <td>ACTIVE</td>
                    <td>PASSIVE</td>
                    <td>DEBIT</td>
                    <td>CREDIT</td>
                    <td>ACTIVE</td>
                    <td>PASSIVE</td>
                </tr>
                {
                    arr.map((e, i) => {
                        //console.log(e.account[0], r)
                        if (e.account[0] !== r && Number(e.account) == e.account) {
                            r = e.account[0]
                            return <tr key={i}><td style={{borderWidth: '3px'}} colSpan={7}>{e.class}</td></tr>
                        } else return (
                            <tr key={i}>
                                <td>{e.account}</td>
                                <td>{e.activeDecimal}</td>
                                <td>{e.passiveDecimal}</td>
                                <td>{e.debitDecimal}</td>
                                <td>{e.creditDecimal}</td>
                                <td>{e.activeDecimal2}</td>
                                <td>{e.passiveDecimal2}</td>
                            </tr>
                        )
                    })
                }
            </table>
        </section>
  );
}

export default Table;